﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace RESTTest
{
    [TestClass]
    public class TestClientes
    {
        [TestMethod]
        public void TestCrearClientes()
        {
            string postdata = "{\"codigo\":\"000002\",\"nombre\":\"HECTOR\",\"direccion\":\"LIMA\"}";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost:1951/Documentos.svc/Documentos");
            req.Method = "POST";
            req.ContentLength = data.Length;
            req.ContentType = "application/json";
            var reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            var res = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string clienteJson = reader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Documento documentoCreado = js.Deserialize<Documento>(postdata);
            Assert.AreEqual("000002", documentoCreado.ruc);
            Assert.AreEqual("HECTOR", documentoCreado.numero_documento);
            Assert.AreEqual("LIMA", documentoCreado.tipo_documento);
        }

        [TestMethod]
        public void TestCrearClienteException()
        {
            string postdata = "{\"codigo\":\"111111\",\"nombre\":\"JUAN\",\"direccion\":\"CUZCO\"}";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost:1951/Documentos.svc/Documentos");
            req.Method = "POST";
            req.ContentLength = data.Length;
            req.ContentType = "application/json";
            var reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            HttpWebResponse res = null;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string documentoJson = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                Documento documentoCreado = js.Deserialize<Documento>(postdata);
                Assert.AreEqual("111111", documentoCreado.ruc);
                Assert.AreEqual("JUAN", documentoCreado.numero_documento);
                Assert.AreEqual("CUZCO", documentoCreado.tipo_documento);
            }
            catch (WebException w)
            {
                HttpStatusCode code = ((HttpWebResponse)w.Response).StatusCode;
                String mensaje = ((HttpWebResponse)w.Response).StatusDescription;
                StreamReader sr = new StreamReader(w.Response.GetResponseStream());
                string error = sr.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensajeException = js.Deserialize<string>(error);
                Assert.AreEqual("El cliente con codigo 111111 ya existe", mensajeException);

            }


        }

        [TestMethod]
        public void TestEliminarProducto()
        {
            // Prueba de eliminacion de producto via HTTP GET
            HttpWebRequest req2 = (HttpWebRequest)WebRequest
                .Create("http://localhost:1951/Documentos.svc/Documentos/123456");
            req2.Method = "DELETE";
            HttpWebResponse res2 = (HttpWebResponse)req2.GetResponse();
            /*
                        // Prueba de obtencion de producto via HTTP GET
                        HttpWebRequest req2 = (HttpWebRequest)WebRequest
                            .Create("http://localhost:1951/Clientes.svc/Clientes/123456");
                        req2.Method = "GET";
            */
            HttpWebResponse res = (HttpWebResponse)req2.GetResponse();
            StreamReader reader2 = new StreamReader(res2.GetResponseStream());
            string documentoJson2 = reader2.ReadToEnd();
            JavaScriptSerializer js2 = new JavaScriptSerializer();
            Documento documentoEliminado = js2.Deserialize<Documento>(documentoJson2);
            Assert.IsNull(documentoEliminado);
        }


        [TestMethod]
        public void TestObtenerCliente()
        {
            HttpWebRequest req2 = (HttpWebRequest)WebRequest.
                Create("http://localhost:1951/Clientes.svc/Clientes/111111");
            req2.Method = "GET";
            HttpWebResponse res2 = (HttpWebResponse)req2.GetResponse();
            StreamReader reader2 = new StreamReader(res2.GetResponseStream());
            string clienteJson2 = reader2.ReadToEnd();
            JavaScriptSerializer js2 = new JavaScriptSerializer();
            Documento clienteObtenido = js2.Deserialize<Documento>(clienteJson2);
            Assert.AreEqual("111111", clienteObtenido.ruc);
            Assert.AreEqual("JUAN", clienteObtenido.numero_documento);
            Assert.AreEqual("CUZCO", clienteObtenido.tipo_documento);
        }

        [TestMethod]
        public void TestModificarCliente()
        {
            string postdata = "{\"codigo\":\"111111\",\"nombre\":\"HHHHHH\",\"direccion\":\"LIMA\"}";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
            HttpWebRequest req = (HttpWebRequest)WebRequest.
                Create("http://localhost:1951/Clientes.svc/Clientes");
            req.Method = "PUT";
            req.ContentLength = data.Length;
            req.ContentType = "application/json";
            var reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            var res = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string clienteJson = reader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Documento documentoModificado = js.Deserialize<Documento>(clienteJson);
            Assert.AreEqual("111111", documentoModificado.ruc);
            Assert.AreEqual("HHHHHH", documentoModificado.numero_documento);
            Assert.AreEqual("LIMA", documentoModificado.tipo_documento);
        }

    }
}

