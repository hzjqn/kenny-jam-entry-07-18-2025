using Godot;
using System;

public partial class PeopleCardElement : Node2D
{
    [Export]
    Card card;

    [Export]
    Node2D body;

    [Export]
    Node2D shadow;

    [Export]
    Label rulesLabel;

    [Export]
    Label valueLabel;

    [Export]
    CardType cardType = CardType.Person;


    [Export]
    AnimationPlayer animationPlayer;

    [Export]

    float rotationSeed = 0;

    TextureButton control;

    Vector2 InitialPosition;


    public override void _Ready()
    {
        InitialPosition = GlobalPosition;
        rotationSeed = (float)GD.RandRange(0, 1000) * 10;
        control = GetNode<TextureButton>("Control");
        control.MouseFilter = Control.MouseFilterEnum.Stop;

        if (control != null)
        {
            control.Pressed += OnCardPressed;
            control.MouseEntered += AniamteHover;
            control.MouseExited += AniamteHoverOut;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        shadow.GlobalRotation = Mathf.Sin((Time.GetTicksMsec() + rotationSeed) / 1000.0f) * 0.05f;
        body.GlobalRotation = shadow.GlobalRotation;

        if (CardManager.selectedCard == card)
        {
            GlobalPosition = GetGlobalMousePosition();
            GlobalScale = new (.5f, .5f);
            control.MouseFilter = Control.MouseFilterEnum.Ignore;
        }
        else
        {
            GlobalPosition = InitialPosition;
            GlobalScale = new (1f, 1f);
            control.MouseFilter = Control.MouseFilterEnum.Stop;
            
        }
    }

    public void OnCardPressed()
    {
        GD.Print("pressed");
        Card selectedCard = CardManager.selectedCard;

        if (card != selectedCard && selectedCard != null)
        {
            OnCardDropped();
        }  
        else if (selectedCard == null)
        {
            CardManager.selectedCard = card;
        }
    }

    public void OnCardDropped()
    {
        if (CardManager.selectedCard != card) return;

        CardManager.useSelectedCardOnHoveredCard();
    }

    public void AniamteHover()
    {
        CardManager.hoveredCard = card;
        animationPlayer.Play("hover");
    }

    public void AniamteHoverOut()
    {
        if (CardManager.hoveredCard == card)
        {
            CardManager.hoveredCard = null;
        }
        animationPlayer.Play("hover_out");
    }
}
