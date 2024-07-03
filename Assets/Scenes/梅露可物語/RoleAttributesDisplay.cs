using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class RoleAttributesDisplay : MonoBehaviour
    {
        public RoleDataObj roleData;
        public GameObject hpShow;
        // Start is called before the first frame update

        void Start()
        {
            if (hpShow == null) { 
                hpShow = gameObject.transform.Find("healthBox").gameObject;
            }
            if(roleData == null)
            {
                roleData = gameObject.transform.Find("entity").gameObject.GetComponent<RoleDataObj>();
            }
            gameObject.transform.Find("roleName").gameObject.GetComponent<Text>().text = roleData.roleData.attributes.name;
        }

        // Update is called once per frame
        void Update()
        {
            hpShow.transform.Find("hptext").gameObject.GetComponent<Text>().text = roleData.roleData.attributes.hp.nowValue.ToString();
            float wV = roleData.roleData.attributes.hp.nowValue / roleData.roleData.attributes.hp.MaxValue();// * 150.0f;
            //hpShow.transform.Find("hp").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(wV, 20);
            hpShow.transform.Find("hp").gameObject.GetComponent<RectTransform>().localScale = new Vector3(wV,1,1);
        }

    }
}
