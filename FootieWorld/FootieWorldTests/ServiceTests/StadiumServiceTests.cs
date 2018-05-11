using Coderful.EntityFramework.Testing.Mock;
using Footieworld.core.Services;
using Footieworld.core.Services.Interfaces;
using FootieWorld.data.ef;
using FootieWorld.data.ef.UnitOfWork;
using LoremNET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootieWorldTests.TestUtils.ServiceTests
{
    [TestClass]
    public class StadiumServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }


        [TestMethod]        
        public void GetAllStadiumsRetuns()
        {
            var dbcontext = UnitTestHelpers.GetUnitTestDBContext();
            
            var unitOfWork = Mock.Of<IUnitOfWork>();

            var stadium = new tblStadium() { StadiumID = Guid.NewGuid() };
            
            
            unitOfWork.Context = dbcontext.Object;
            var serviceUnderTest = new StadiumService(unitOfWork);
            var result = serviceUnderTest.GetAllStadiums();

            Assert.IsNotNull(result);
        }

        
    }
}
