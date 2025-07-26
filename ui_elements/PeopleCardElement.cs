using Godot;
using System;
using System.Collections.Generic;

public partial class PeopleCardElement : Node2D
{
    GameManager gm;
    CardManager cm;
    UiManager uim;


    [Export]
    public Card card;

    [Export]
    Node2D body;

    [Export]
    Node2D shadow;

    [Export]
    Label rulesLabel;

    [Export]
    Label valueLabel;
    [Export]
    Sprite2D suitIconSprite;
    [Export]
    Sprite2D suitPersonSprite;
    [Export]
    Sprite2D actionTypeIconSprite;
    [Export]
    Sprite2D actionIconSprite;
    [Export]
    CardType cardType = CardType.Person;


    [Export]
    AnimationPlayer animationPlayer;

    [Export]

    float rotationSeed = 0;
    public bool enabled = false;

    TextureButton control;

    Vector2 InitialPosition;


    public override void _Ready()
    {
        gm = GetNode<GameManager>("/root/GameManager");
        cm = GetNode<CardManager>("/root/CardManager");
        uim = GetNode<UiManager>("/root/UiManager");
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

        if (cm.selectedCard == card)
        {
            GlobalPosition = GetGlobalMousePosition();
            GlobalScale = new(.5f, .5f);
            control.MouseFilter = Control.MouseFilterEnum.Ignore;
        }
        else
        {
            GlobalPosition = InitialPosition;
            GlobalScale = new(1f, 1f);
            control.MouseFilter = Control.MouseFilterEnum.Stop;

        }

        if (card == null) return;
        if (suitIconSprite != null)
            suitIconSprite.Texture = uim.cardSuitIconTextures[card.cardSuit];
        if (suitPersonSprite != null)
            suitPersonSprite.Texture = uim.cardSuitPersonTextures[card.cardSuit];
        if (valueLabel != null)
            valueLabel.Text = card.value.ToString();
        if (rulesLabel != null)
            rulesLabel.Text = card.rules.ToString();
        if (actionIconSprite != null)
            switch (card.cardType)
            {
                case CardType.BasicAction:
                    if (actionIconSprite != null)
                        actionIconSprite.Texture = uim.basicActionTypeIconTextures[(BasicActionTypes)card.value];
                    break;
                case CardType.SpecialAction:
                    if (actionIconSprite != null)
                        actionIconSprite.Texture = uim.specialActionTypeIconTextures[(SpecialActionTypes)card.value];
                    break;
            }
        if (actionTypeIconSprite != null)
            switch (card.cardType)
            {
                case CardType.BasicAction:
                    if (actionTypeIconSprite != null)
                        actionTypeIconSprite.Texture = uim.basicIcontexture;
                    break;
                case CardType.SpecialAction:
                    if (actionTypeIconSprite != null)
                        actionTypeIconSprite.Texture = uim.speicalIconTexture;
                    break;
            }
        if (card.cardType != CardType.Person)
        {
            if (valueLabel != null)
            {
                if (card.cardType == CardType.BasicAction)
                    valueLabel.Text = UiManager.GetBasicActionName((BasicActionTypes)card.value);
                if (card.cardType == CardType.SpecialAction)
                    valueLabel.Text = UiManager.GetSpecialActionName((SpecialActionTypes)card.value);
            }
        }
    }

    public void OnCardPressed()
    {
        if (!enabled)
        {
            GD.Print(Name, "card is not enabled");
            return;
        }
        Card selectedCard = cm.selectedCard;

        if (card != selectedCard && selectedCard != null)
        {
            OnCardDropped();
        }
        else if (selectedCard == null)
        {
            if (card.cardType == CardType.Person) return;
            cm.selectedCard = card;
        }
    }

    public void OnCardDropped()
    {
        GD.Print("OnCardDropped");
        if (!enabled) return;
        if (cm.selectedCard == card) return;

        cm.useSelectedCardOnHoveredCard();
    }

    public void AniamteHover()
    {
        if (!enabled) return;
        cm.hoveredCard = card;
        animationPlayer.Play("hover");
    }

    public void AniamteHoverOut()
    {
        if (!enabled) return;
        if (cm.hoveredCard == card)
        {
            cm.hoveredCard = null;
        }
        animationPlayer.Play("hover_out");
    }

    public void Appear()
    {
        GD.Print("Appeared", card);
        enabled = true;
        animationPlayer.Play("appear");
    }
    public void Disappear()
    {
        enabled = false;
        animationPlayer.Play("disappear");
    }
}
