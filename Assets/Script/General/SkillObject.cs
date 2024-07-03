using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class SkillObject
    {
        public RoleSkill skill;
        public float nowCoolDown;//技能現在冷卻值
        public bool prepareToAttack;//是否能攻擊
        private Transform userPosition;
        private RoleAttributes a;

        public SkillObject(RoleAttributes a,Transform userPosition,RoleSkill roleSkill)
        {
            this.userPosition = userPosition;
            this.a = a;
            skill = roleSkill;
            nowCoolDown = roleSkill.attackCoolDown;
            prepareToAttack = true;
        }

        /**
         * 計算傷害
         */
        public float Damage(RoleAttributes b)
        {
            return (a.physicalAttack.Value() - b.physicalDefense.Value());
        }

        /**
         * 技能冷卻
         */
        public void CoolDown(float coolDownValueBySecond)
        {
            if(prepareToAttack) { return; }
            nowCoolDown -= coolDownValueBySecond*Time.deltaTime;
            if (nowCoolDown <= 0) {
                nowCoolDown = 0;
                prepareToAttack = true;
            }
        }

        /*
         * 對 目標 使用技能
         */
        public void UseSkill(GameObject b)
        {
            if (!prepareToAttack) { return; }
            RoleAttributes b_Attributes = b.GetComponent<RoleAttributes>();
            if (b_Attributes != null)
            {
                b_Attributes.hp.nowValue -= Damage(b_Attributes);
            }
            prepareToAttack = false;
            nowCoolDown = skill.attackCoolDown;
        }

        public void UseSkill(List<GameObject> bs)
        {
            if (!prepareToAttack) { return; }
            foreach(GameObject b in bs)
            {
                RoleAttributes b_Attributes = b.GetComponent<RoleAttributes>();
                if (b_Attributes != null)
                {
                    b_Attributes.hp.nowValue -= Damage(b_Attributes);
                }
            }
            prepareToAttack = false;
            nowCoolDown = skill.attackCoolDown;
        }

        public List<GameObject> SelecAttackTaget(List<GameObject> bs)
        {
            List<GameObject> result = new List<GameObject>();
            float minD = float.MaxValue;
            float maxD = float.MinValue;
            bs.Sort((a, b) =>
            {
                float thisToa = (a.transform.position - userPosition.position).magnitude;
                float thisTob = (b.transform .position - userPosition.position).magnitude;
                float nowMin = thisToa;
                float nowMax = thisTob;
                if(thisToa > thisTob) { nowMin = thisTob; nowMax = thisToa; }
                if(nowMin < minD)minD = nowMin;
                if(nowMax > maxD)maxD = nowMax;
                return (int)(thisToa - thisTob);
            });

            foreach(GameObject b in bs)
            {
                switch (skill.attackMode)
                {
                    case AttackMode.Rays:
                        if((b.transform.position - userPosition.position).magnitude == minD)
                        {
                            result.Add(b);
                        }
                        break;
                    case AttackMode.Scope: 
                        result.Add(b); break;
                    case AttackMode.Parabola:
                        result.Add(b); break;
                }
            }
            return result;
        }
    }
}
