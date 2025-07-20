#!/usr/bin/env python3
"""
GTK front-end for GameManager.

Backend file must be importable (e.g.  `import game_backend as gb`)
and **must not be modified**.  All interaction happens through:

* builtins.input (…)    –- replaced by a function that pulls values
                           from a thread-safe Queue, populated when
                           the user clicks buttons in the UI.
* GameManager.print_state –- monkey-patched so that the state is
                           shown in the window instead of stdout.
"""
import threading
import queue
import builtins
import gi

gi.require_version("Gtk", "3.0")            #  GTK 3; change to "4.0" if needed
from gi.repository import Gtk, GLib

# ----------------------------------------------------------------------
#  ░█░█░▀█▀░█▄█░█▀█░▀█▀   util
# ----------------------------------------------------------------------
def safe_idle(func, *args):
    """Schedule *func* on the GTK main thread, ignore if app is closing."""
    try:
        GLib.idle_add(func, *args, priority=GLib.PRIORITY_DEFAULT)
    except RuntimeError:
        pass


# ----------------------------------------------------------------------
#  ░█▀▀░█▀█░█▀█░█▀▀   GUI
# ----------------------------------------------------------------------
class GameWindow(Gtk.Window):
    def __init__(self, action_queue):
        super().__init__(title="Card-Kingdom GTK")
        self.set_resizable(False)
        self.action_queue = action_queue

        self.selected_action = None
        self.selected_card = None

        # --- layout ----------------------------------------------------
        vbox = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=6)
        vbox.set_border_width(8)
        self.add(vbox)

        self.state_label = Gtk.Label(xalign=0)
        self.state_label.set_margin_bottom(6)
        vbox.pack_start(self.state_label, False, False, 0)

        # grids that will be rebuilt every time state changes
        self.actions_grid = Gtk.Grid(column_spacing=4, row_spacing=4)
        self.people_grid  = Gtk.Grid(column_spacing=4, row_spacing=4)

        vbox.pack_start(Gtk.Label(label="Actions:"), False, False, 0)
        vbox.pack_start(self.actions_grid,   False, False, 0)
        vbox.pack_start(Gtk.Label(label="People:"),  False, False, 0)
        vbox.pack_start(self.people_grid,    False, False, 0)

        self.show_all()

    # ---------- helpers -----------------
    def button_maker(self, text, idx, kind):
        """Produce a Gtk.Button that remembers what it represents."""
        btn = Gtk.Button(label=text)
        btn.props.name = f"{kind}:{idx}"
        btn.connect("clicked", self.on_btn_clicked, idx, kind)
        return btn

    def on_btn_clicked(self, _button, idx, kind):
        """Store the user’s choice; when both parts are chosen, emit to queue."""
        if kind == "action":
            self.selected_action = idx
        else:
            self.selected_card = idx

        if self.selected_action is not None and self.selected_card is not None:
            # Send to backend – note the order expected by GameManager.main
            self.action_queue.put(str(self.selected_action))
            self.action_queue.put(str(self.selected_card))
            # reset UI selections
            self.selected_action = self.selected_card = None

    # ---------- exposed to GameManager -----------------
    def render_state(self, state_dict):
        """
        Called from the patched GameManager.print_state.
        Rebuild labels & buttons with the data contained in *state_dict*.
        """
        # 1) summary line
        self.state_label.set_text(
            "Deck: {deck_size} | gold {monedas} | popularity {popularidad} | "
            " strength {fuerza} | expenses {gasto}".format(**state_dict)
        )

        # 2) actions ----------------------------------------------------
        for child in list(self.actions_grid):
            self.actions_grid.remove(child)

        actions_room = state_dict.get("actions_room", {})
        for col, (idx, action) in enumerate(actions_room.items()):
            label = "—"
            if action is not None:
                label = f"{idx}: {action[0]}"
            btn = self.button_maker(label, idx, "action")
            btn.set_sensitive(action is not None)
            self.actions_grid.attach(btn, col, 0, 1, 1)

        # 3) people -----------------------------------------------------
        for child in list(self.people_grid):
            self.people_grid.remove(child)

        people_room = state_dict.get("people_room", {})
        for col, (idx, card) in enumerate(people_room.items()):
            label = "—"
            if card is not None:
                value, suit = card
                label = f"{idx}: {value}{suit}"
            btn = self.button_maker(label, idx, "card")
            btn.set_sensitive(card is not None)
            self.people_grid.attach(btn, col, 0, 1, 1)

        self.show_all()                       # refresh everything


# ----------------------------------------------------------------------
#  ░█▄█░█▀█░█▀▀░█▀▀   glue to backend
# ----------------------------------------------------------------------
def main():
    import prototipo as gb              # ←——  your backend module name

    # Thread-safe queue where GUI puts answers, backend consumes via input()
    q = queue.Queue()

    # Monkey-patch builtins.input BEFORE GameManager is instantiated
    def gui_input(prompt: str = "") -> str:          # noqa: D401
        # prompt ignored – backend displays it anyway, we render separately
        return q.get()                               # blocks until GUI fills
    builtins.input = gui_input

    # ------------------------------------------------------------------
    #  bootstrap window (must exist before we patch print_state)
    # ------------------------------------------------------------------
    win = GameWindow(q)

    # Monkey-patch GameManager.print_state so it forwards info to the window
    def patched_print_state(self):
        state = {
            "deck_size": len(self.mazo_personajes),
            "monedas": self.monedas,
            "popularidad": self.popularidad,
            "gasto": self.gasto,
            "fuerza": self.fuerza,
            "people_room": getattr(self, "people_room", {}),
            "actions_room": getattr(self, "actions_room", {}),
        }
        safe_idle(win.render_state, state)
    gb.GameManager.print_state = patched_print_state

    # -- run backend in its own thread so the GTK loop stays responsive ----
    def backend_thread():
        gm = gb.GameManager()
        gm.main()                               # blocks until game ends
        safe_idle(win.set_title, "Game Over!")

    threading.Thread(target=backend_thread, daemon=True).start()

    # standard GTK run
    win.connect("destroy", Gtk.main_quit)
    Gtk.main()


if __name__ == "__main__":
    main()
