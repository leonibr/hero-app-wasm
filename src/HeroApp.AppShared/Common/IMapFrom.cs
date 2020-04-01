using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Common
{
    public interface IMapFrom<T> where T: class
    {
        void Mapping(AutoMapper.Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
