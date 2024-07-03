using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class RoleCardOperate : MonoBehaviour
    {
        public RoleAI operateRole;
        public Vector3 mouseDownP;
        public bool fie;
        public bool dragIs;
        // Start is called before the first frame update
        void Start()
        {
            fie = false;
            dragIs= false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetOperateRole(RoleAI a)
        {
            operateRole = a;
        }

        private void OnMouseDown()
        {
            mouseDownP.y = Input.mousePosition.y;
        }

        private void OnMouseDrag()
        {
            dragIs = true;
            if (mouseDownP.y - Input.mousePosition.y > 0) { 
                fie = true;
            }
            else
            {
                fie = false;
            }
        }

        private void OnMouseUp()
        {
            if (dragIs)
            {
                if(fie) {
                    operateRole.Forward(); // = false;

                }
                else
                {
                    operateRole.Backward();// = true;
                }
                dragIs = false;
            }
            
        }
    }
}
