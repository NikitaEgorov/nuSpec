﻿using FluentNHibernate.Mapping;

namespace nuSpec.Tests.Models
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            this.Id(x => x.Id).GeneratedBy.Identity();
            this.Map(x => x.Name);
        }
    }
}
