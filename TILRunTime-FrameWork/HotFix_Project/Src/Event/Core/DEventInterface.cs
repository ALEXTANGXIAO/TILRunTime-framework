using System;

namespace HotFix_Project
{
    public enum DEventGroup
    {
        GroupUI,
        GroupLogic,
    }

    [System.AttributeUsage(System.AttributeTargets.Interface)]
    public class DEventInterface : Attribute
    {
        public DEventGroup m_group;
        public DEventInterface(DEventGroup group)
        {
            m_group = group;
        }
    }
}
