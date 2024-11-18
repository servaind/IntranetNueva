using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class PermisoPersonaTests : TestBase
    {
        [TestCase]
        public void UpdateTest()
        {
            List<PermisosPersona> permisos = new List<PermisosPersona>
            {
                PermisosPersona.AdminImputaciones,
                PermisosPersona.AdminPartesDiarios,
                PermisosPersona.AdminPersonal
            };

            try
            {
                PermisoPersona.Update(89, permisos);
            }
            catch
            {
                Assert.Fail();
            }

            Persona p = Persona.Read(89);
            Assert.AreEqual(permisos.Count, p.Permisos.Count);

            permisos.ForEach(permiso => Assert.IsTrue(p.Permisos.Contains(permiso)));
        }
    }
}
