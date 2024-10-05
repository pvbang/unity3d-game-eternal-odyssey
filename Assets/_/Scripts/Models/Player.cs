using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Player 
{
    public string playerId;
    public string name;                 // Tên nhân vật
    public Attribute attributes;        // Chỉ số của nhân vật
    public List<Skills> skills;         // Danh sách skill của nhân vật
    public PlayerClass playerClass;     // Class của nhân vật
    public Element properties;          // Thuộc tính của nhân vật (Kim, Mộc, Thủy, Hỏa, Thổ)
    public int level;                   // Level của nhân vật
    public int exp;                     // Số kinh nghiệm của nhân vật
    public List<string> belt;           // Danh sách vũ khí
    public List<string> cloth;          // Danh sách áo giáp
    public List<string> crown;          // Danh sách mũ
    public List<string> face;           // Danh sách mặt nạ
    public List<string> glove;          // Danh sách găng tay
    public List<string> hair;           // Danh sách tóc
    public List<string> hat;            // Danh sách mũ
    public List<string> helm;           // Danh sách mũ bảo hiểm
    public List<string> shoe;           // Danh sách giày
    public List<string> shoulderPad;    // Danh sách bảo vệ vai

    public Player()
    { }

    public Player(string playerId, string name, Attribute attributes, List<Skills> skills, PlayerClass playerClass, Element properties, int level, int exp, List<string> belt, List<string> cloth, List<string> crown, List<string> face, List<string> glove, List<string> hair, List<string> hat, List<string> helm, List<string> shoe, List<string> shoulderPad)
    {
        this.playerId = playerId;
        this.name = name;
        this.attributes = attributes;
        this.skills = skills;
        this.playerClass = playerClass;
        this.properties = properties;
        this.level = level;
        this.exp = exp;
        this.belt = belt;
        this.cloth = cloth;
        this.crown = crown;
        this.face = face;
        this.glove = glove;
        this.hair = hair;
        this.hat = hat;
        this.helm = helm;
        this.shoe = shoe;
        this.shoulderPad = shoulderPad;
    }

    public Player(string name, Attribute attributes, List<Skills> skills, PlayerClass playerClass, Element properties, int level, int exp, List<string> belt, List<string> cloth, List<string> crown, List<string> face, List<string> glove, List<string> hair, List<string> hat, List<string> helm, List<string> shoe, List<string> shoulderPad)
    {
        this.name = name;
        this.attributes = attributes;
        this.skills = skills;
        this.playerClass = playerClass;
        this.properties = properties;
        this.level = level;
        this.exp = exp;
        this.belt = belt;
        this.cloth = cloth;
        this.crown = crown;
        this.face = face;
        this.glove = glove;
        this.hair = hair;
        this.hat = hat;
        this.helm = helm;
        this.shoe = shoe;
        this.shoulderPad = shoulderPad;
    }



}
