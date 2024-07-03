using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoraHareSakura_General
{
    public class RoleAI : MonoBehaviour
    {
        public bool moveV;//²¾°Ê¤è¦V true
        public RoleDataObj entityData;
        public bool noAttack;
        public bool backMove;

        public List<GameObject> teammate;
        public List<GameObject> enemys;

        public bool fighetEnd;

        // Start is called before the first frame update
        void Start()
        {
            enemys = new List<GameObject>();
            entityData = gameObject.transform.Find("entity").GetComponent<RoleDataObj>();
            noAttack = true;
            backMove = false;
            fighetEnd = false;
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.SetActive(!entityData.LoseTheAbilityToFight());
            if(fighetEnd ) { return; }
            if (backMove)
            {
                if (moveV)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = 3 * entityData.roleData.attributes.speed.Value() * Vector3.left;
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = 3 * entityData.roleData.attributes.speed.Value() * Vector3.right;
                }
            }
            else if (noAttack)
            {
                if (moveV)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = 3 * entityData.roleData.attributes.speed.Value() * Vector3.right;
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = 3 * entityData.roleData.attributes.speed.Value() * Vector3.left;
                }
            }
        }

        public void LoadData(RoleDataObj roleData)
        {
            entityData = roleData;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("enter"+other.gameObject.name);//other.gameObject.GetComponent<RoleData>()
            if(other.gameObject.name == "entity" && other.gameObject.GetComponent<RoleDataObj>().isEnemy != entityData.isEnemy)
            {
                //enemys.Add(other.gameObject);
                SetAttackQueue(other.gameObject);
                entityData.UseSkill(teammate, enemys);
                noAttack = false;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("Stay"+other.gameObject.name);//other.gameObject.GetComponent<RoleData>()
            if (other.gameObject.name == "entity" && other.gameObject.GetComponent<RoleDataObj>().isEnemy != entityData.isEnemy)
            {
                entityData.UseSkill(teammate, enemys);
                noAttack = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            enemys.Remove(collision.gameObject);
            if (collision.gameObject.name == "entity")
            {
                noAttack = true;
            }
        }

        public void Forward()
        {
            backMove = false;
        }

        public void Backward()
        {
            backMove = true;
        }

        public bool Befeated()
        {
            return entityData.LoseTheAbilityToFight();
        }

        public void FighetEnd()
        {
            fighetEnd = true;
        }

        public void SetAttackQueue(GameObject enemyObj)
        {
            enemys.Add(enemyObj);
        }
    }
}
