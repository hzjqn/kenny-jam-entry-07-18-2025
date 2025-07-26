using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class UiManager : Node
{
    [Export] public Color militiaSuitColor;
    [Export] public Color clergySuitColor;
    [Export] public Color opponetSuitColor;
    [Export] public Color merchantSuitColor;
    [Export] public Texture2D militiaIconTexture;
    [Export] public Texture2D militiaPersonTexture;
    [Export] public Texture2D clerigyIconTexture;
    [Export] public Texture2D merchantIconTexture;
    [Export] public Texture2D opponentIconTexture;
    [Export] public Texture2D clerigyPersonTexture;
    [Export] public Texture2D merchantPersonTexture;
    [Export] public Texture2D opponentPersonTexture;
    [Export] public Texture2D convertIconTexture;
    [Export] public Texture2D speicalIconTexture;
    [Export] public Texture2D basicIcontexture;
    [Export] public Texture2D threatIconTexture;
    [Export] public Texture2D bribeIconTexture;
    [Export] public Texture2D taxIconTexture;
    [Export] public Texture2D ignoreIconTexture;
    public Dictionary<CardSuits, Texture2D> cardSuitIconTextures;
    public Dictionary<CardSuits, Texture2D> cardSuitPersonTextures;
    public Dictionary<CardSuits, Color> cardSuitColors;
    public Dictionary<BasicActionTypes, Texture2D> basicActionTypeIconTextures;
    public Dictionary<SpecialActionTypes, Texture2D> specialActionTypeIconTextures;

    public override void _Ready()
    {
        base._Ready();
        cardSuitColors = new Dictionary<CardSuits, Color>
        {
            {CardSuits.Clergy, clergySuitColor},
            {CardSuits.Merchant, merchantSuitColor},
            {CardSuits.Militia, militiaSuitColor},
            {CardSuits.Opponent, opponetSuitColor}
        };
        cardSuitIconTextures = new Dictionary<CardSuits, Texture2D>
        {
            {CardSuits.Clergy, clerigyIconTexture},
            {CardSuits.Merchant, merchantIconTexture},
            {CardSuits.Militia, militiaIconTexture},
            {CardSuits.Opponent, opponentIconTexture}
        };
        cardSuitPersonTextures = new Dictionary<CardSuits, Texture2D>
        {
            {CardSuits.Clergy, clerigyPersonTexture},
            {CardSuits.Merchant, merchantPersonTexture},
            {CardSuits.Militia, militiaPersonTexture},
            {CardSuits.Opponent, opponentPersonTexture}
        };
        basicActionTypeIconTextures = new Dictionary<BasicActionTypes, Texture2D>
        {
            {BasicActionTypes.Bribe, bribeIconTexture},
            {BasicActionTypes.Ignore, ignoreIconTexture},
            {BasicActionTypes.Tax, taxIconTexture},
            {BasicActionTypes.Threat, threatIconTexture}
        };
        specialActionTypeIconTextures = new Dictionary<SpecialActionTypes, Texture2D>
        {
            {SpecialActionTypes.Convert, convertIconTexture}
        };
    }


    public static string GetCardSuitName(CardSuits suit)
    {
        return suit switch
        {
            CardSuits.Clergy => "Clergy",
            CardSuits.Militia => "Militia",
            CardSuits.Merchant => "Merchant",
            CardSuits.Opponent => "Opponent",
            _ => "Unkown"
        };
    }

    public static string GetBasicActionName(BasicActionTypes action)
    {
        return action switch
        {
            BasicActionTypes.Bribe => "Bribe",
            BasicActionTypes.Tax => "Tax",
            BasicActionTypes.Ignore => "Ignore",
            BasicActionTypes.Threat => "Threat",
            _ => "Undefined"
        };
    }
    public static string GetSpecialActionName(SpecialActionTypes action)
    {
        return action switch
        {
            SpecialActionTypes.Convert => "Convert",
            _ => "Undefined"
        };
    }
}
