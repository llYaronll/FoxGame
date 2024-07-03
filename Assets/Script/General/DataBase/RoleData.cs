using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class RoleData
    {
        public RoleAttributes attributes;
        public List<RoleSkill> skills;

        public RoleData(RoleData roleData)
        {
            attributes = roleData.attributes;
            skills = roleData.skills;
        }

        public RoleData(RoleAttributes roleAttributes)
        {
            attributes = roleAttributes;
        }
    }
}
