using AutoMapper;
using MenuManagement.Core.Common.Mapping;

namespace MenuManagement.Core.Test.Base
{
    public class BaseMockSetup
    {
        public Mapper _mapper;
        public BaseMockSetup()
        {
            var config = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

            _mapper = new Mapper(config);
        }
    }
}
