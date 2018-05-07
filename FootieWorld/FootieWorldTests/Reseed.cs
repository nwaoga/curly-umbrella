using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootieWorld.data.ef;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using LoremNET;



namespace FootieWorldTests
{
    [TestClass]
    public class Reseed : Testbase
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        
        [TestMethod]
        [Ignore]
        public void ReseedDb()
        {
            this.reseedDb();
        }


    }
}
