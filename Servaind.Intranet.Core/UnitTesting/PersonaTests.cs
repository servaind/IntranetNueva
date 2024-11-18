using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NUnit.Framework;
using Proser.Common;
using Servaind.Intranet.Core.Helpers;

namespace Servaind.Intranet.Core.UnitTesting
{
    [TestFixture]
    public class PersonaTests : TestBase
    {
        [TestCase]
        public void ReadTest()
        {
            Assert.IsNull(Persona.Read(0));
            
            Persona p = Persona.Read(89);
            Assert.IsNotNull(p);

            Assert.AreEqual(89, p.Id);
            Assert.AreEqual("DURAN, MARTIN", p.Nombre);
            Assert.AreEqual("martin.duran@servaind.com", p.Email);
            Assert.AreEqual("martin.duran", p.Usuario);
            Assert.AreEqual(75, p.ResponsableId);
            Assert.AreEqual("IGLESIAS, GERMAN", p.Responsable.Nombre);
            Assert.AreEqual(true, p.EnPanelControl);
            Assert.AreEqual(true, p.Activo);
            Assert.AreEqual("20333991501", p.Cuil);
            Assert.AreEqual(8, p.HoraEntrada.Hour);
            Assert.AreEqual(30, p.HoraEntrada.Minute);
            Assert.AreEqual(17, p.HoraSalida.Hour);
            Assert.AreEqual(30, p.HoraSalida.Minute);

            List<PermisosPersona> permisos = p.Permisos;
            Assert.AreEqual(25, permisos.Count);

            p = Persona.Read("martin.duran");
            Assert.IsNotNull(p);
            Assert.AreEqual(89, p.Id);
        }

        [TestCase]
        public void ListTest()
        {
            UsuarioHelper.Login(89);

            var lst1 = Persona.ListActivas();
            Assert.AreEqual(82, lst1.Count);

            var lst2 = Persona.List();
            Assert.AreEqual(320, lst2.Count);

            var lst3 = Persona.List(new[] {75, 89});
            Assert.AreEqual(2, lst3.Count);

            var lst4 = Persona.ListFromResponsable(75, true);
            Assert.AreEqual(2, lst4.Count);

            var lst5 = Persona.ListFromResponsable(75, false);
            Assert.AreEqual(0, lst5.Count);

            var lst6 = Persona.ListFromResponsable(75, true, false);
            Assert.AreEqual(2, lst6.Count);
        }

        [TestCase]
        public void ListPermisoTest()
        {
            var lst1 = PermisoPersona.ListPersonas(PermisosPersona.Administrador);
            Assert.AreEqual(2, lst1.Count);

            var lst2 = PermisoPersona.ListPersonas(PermisosPersona.AdminPersonal);
            Assert.AreEqual(2, lst2.Count);
        }

        [TestCase]
        public void CreateTest()
        {
            try
            {
                Persona.Create(String.Empty, String.Empty, String.Empty, Constants.InvalidInt, true, true, String.Empty,
                    String.Empty, String.Empty, String.Empty, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Persona.Create("A", String.Empty, String.Empty, Constants.InvalidInt, true, true, String.Empty,
                    String.Empty, String.Empty, String.Empty, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Persona.Create("A", "B", String.Empty, Constants.InvalidInt, true, true, String.Empty,
                    String.Empty, String.Empty, String.Empty, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            try
            {
                Persona.Create("A", "B", "C", Constants.InvalidInt, true, true, String.Empty,
                    String.Empty, String.Empty, String.Empty, Constants.InvalidInt);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }

            string nombre = "Test";
            string email = "test@servaind.com";
            string usuario = "test";
            int responsableId = 75;
            bool enPanelControl = true;
            bool activo = false;
            string legajo = "12345";
            string cuil = "20-888888-1";
            string hEntrada = "8:30";
            string hSalida = "1730";
            int baseId = 1;

            try
            {
                Persona.Create(nombre, email, usuario, responsableId, enPanelControl, activo, legajo,
                    cuil, hEntrada, hSalida, baseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Persona p = Persona.Read(usuario);
            Assert.IsNotNull(p);

            Assert.AreEqual(nombre, p.Nombre);
            Assert.AreEqual(email, p.Email);
            Assert.AreEqual(usuario, p.Usuario);
            Assert.AreEqual(responsableId, p.ResponsableId);
            Assert.AreEqual(enPanelControl, p.EnPanelControl);
            Assert.AreEqual(activo, p.Activo);
            Assert.AreEqual(cuil, p.Cuil);
            Assert.AreEqual(8, p.HoraEntrada.Hour);
            Assert.AreEqual(30, p.HoraEntrada.Minute);
            Assert.AreEqual(17, p.HoraSalida.Hour);
            Assert.AreEqual(30, p.HoraSalida.Minute);
        }

        [TestCase]
        public void UpdateTest()
        {
            int id = 89;
            string nombre = "Test-Test";
            string email = "test1@servaind.com";
            string usuario = "test2";
            int responsableId = 56;
            bool enPanelControl = false;
            bool activo = true;
            string legajo = "12345";
            string cuil = "20-888888-1";
            string hEntrada = "9:30";
            string hSalida = "18:30";
            int baseId = 2;

            try
            {
                Persona.Update(id, nombre, email, usuario, responsableId, enPanelControl, activo, legajo,
                    cuil, hEntrada, hSalida, baseId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Persona p = Persona.Read(id);
            Assert.IsNotNull(p);

            Assert.AreEqual(nombre, p.Nombre);
            Assert.AreEqual(email, p.Email);
            Assert.AreEqual(usuario, p.Usuario);
            Assert.AreEqual(responsableId, p.ResponsableId);
            Assert.AreEqual(enPanelControl, p.EnPanelControl);
            Assert.AreEqual(activo, p.Activo);
            Assert.AreEqual(cuil, p.Cuil);
            Assert.AreEqual(9, p.HoraEntrada.Hour);
            Assert.AreEqual(30, p.HoraEntrada.Minute);
            Assert.AreEqual(18, p.HoraSalida.Hour);
            Assert.AreEqual(30, p.HoraSalida.Minute);
        }
    }
}
