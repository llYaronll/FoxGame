using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

namespace SoraHareSakura_General
{

    [System.Serializable]
    public class RoleDataObj : MonoBehaviour
    {
        public string roleId;
        public string roleType;
        public bool isEnemy;
        public RoleData roleData;
        public List<SkillObject> skillObjs;

        // Start is called before the first frame update
        void Start()
        {
            skillObjs = new List<SkillObject> ();

            roleData.skills.ForEach(skill => {
                skillObjs.Add(new SkillObject(roleData.attributes, transform,skill));
            });
        }

        public void FixedUpdate()
        {
            //�N�o�ޯ�
            foreach (SkillObject item in skillObjs)
            {
                item.CoolDown(roleData.attributes.speed.nowValue);
            }
        }

        public void LoadData(RoleData roleData, string roleType, string roleId)
        {
            this.roleData = new RoleData(roleData);
            this.roleType = roleType;
            this.roleId = roleId;
        }

        /**
         * �ϥΧޯ�ҨϥΪ��禡
         */
        public void UseSkill(List<GameObject> T,List<GameObject> b) {
            //�N�C�ӧޯೣ�����i�W�ߵo��
            //�o�ʧޯ�õ��ޯ�n�o�ʪ���H
            foreach(SkillObject skill in skillObjs)
            {
                switch (skill.skill.type)
                {
                    case TargetType.enemy:
                        skill.UseSkill(b);
                        break;
                    case TargetType.teammate:
                        skill.UseSkill(T);
                        break;
                    case TargetType.user: 
                        skill.UseSkill(gameObject);
                        break;
                }
            }
        }

        /*
         * �O�_����԰�? true ����԰� false �i�H�԰�
         */
        public bool LoseTheAbilityToFight()
        {
            if (roleData.attributes.hp.nowValue > 0)
            {
                return false;
            }
            return true;
        }
    }
}
