using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_General
{
    public enum Nature
    {
        水,
        火,
        植物,
        冰,
        毒,
        暗,
        光,
        無屬性
    }

    public enum AttackMode
    {
        Rays,
        Scope,
        Parabola
    };

    public enum TargetType
    {
        enemy,
        teammate,
        user
    }

    [System.Serializable]
    public class RoleSkill
    {
        public string name;
        public string description;
        public Nature nature;
        public TargetType type;
        public AttackMode attackMode;
        public float attackCoolDown;
        public float attackRange;
        public int damageRange;


        public RoleSkill()
        {

        }

        public RoleSkill(string name, string description, Nature nature, TargetType type, AttackMode attackMode, float attackCoolDown, float attackRange, int damageRange)
        {
            this.name = name;
            this.description = description;
            this.nature = nature;
            this.type = type;
            this.attackMode = attackMode;
            this.attackCoolDown = attackCoolDown;
            this.attackRange = attackRange;
            this.damageRange = damageRange;
        }

        public RoleSkill(RoleSkill roleSkill)
        {
            name = roleSkill.name;
            description = roleSkill.description;
            attackCoolDown = roleSkill.attackCoolDown;
            attackRange = roleSkill.attackRange;
            damageRange = roleSkill.damageRange;
            attackMode = roleSkill.attackMode;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
