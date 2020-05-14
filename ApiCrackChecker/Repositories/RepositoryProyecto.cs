using ApiCrackChecker.Data;
using ApiCrackChecker.Helpers;
using NuGetCrackChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrackChecker.Repositories
{
    public class RepositoryProyecto : IRepositoryProyecto
    {
        ProyectoContext context;
        public RepositoryProyecto(ProyectoContext context)
        {
            this.context = context;
        }

        public Juego BuscarJuegoSlug(string slug)
        {
            return this.context.Juegos.SingleOrDefault(z => z.Slug == slug);
        }

        public Juego BuscarJuegoId(string id)
        {
            return this.context.Juegos.SingleOrDefault(z => z.IdJuego == id);
        }

        public void EditarJuego(String id, DateTime fechaCrack,String fechaLanz)
        {
            Juego j = this.BuscarJuegoId(id);
            j.FechaCrack = fechaCrack;
            j.FechaLanzamiento = fechaLanz;
            this.context.SaveChanges();
        }

        public Usuario BuscarUsuario(int id)
        {
            return this.context.Usuarios.SingleOrDefault(z => z.IdUsuario == id);
        }
        public Usuario BuscarUsuario(string email)
        {
            return this.context.Usuarios.SingleOrDefault(z => z.Email == email);
        }

        public Usuario BuscarUsuario(string email, string password)
        {
            Usuario u = this.context.Usuarios.SingleOrDefault(z => z.Email == email);
            if (u != null)
            {
                byte[] passcifrado = CypherHelper.CifradoHashSHA256(password + u.Salt);
                if (CypherHelper.CompararBytes(u.Password, passcifrado))
                {
                    return u;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void ActivarCuenta(int id)
        {
            Usuario u = this.BuscarUsuario(id);
            if (u != null)
            {
                u.Activo = 1;
                this.context.SaveChanges();
            }
        }

        public List<Usuario> BuscarUsuariosPorRole(string role)
        {
            return this.context.Usuarios.Where(z => z.Role == role).ToList();
        }

        public void EditarUsuario(int id, string email, string role, string foto, string nombre, string apellidos, DateTime fechaNac, int tlf, string username)
        {
            Usuario u = this.BuscarUsuario(id);
            String salt = u.Salt;
            u.Email = email;
            u.Role = role;
            if (foto != null)
            {
                foto = foto.Replace("\"", "");
                u.Foto = foto;
            }
            u.Nombre = nombre;
            u.Apellidos = apellidos;
            u.FechaNacimiento = fechaNac;
            u.Telefono = tlf;
            u.Username = username;
            this.context.SaveChanges();
        }

        public void EditarUsuario(int id, String role, int activo)
        {
            Usuario u = this.BuscarUsuario(id);
            u.Role = role;
            u.Activo = activo;
            this.context.SaveChanges();
        }

        public List<Juego> GetJuegos()
        {
            return this.context.Juegos.ToList();
        }

        public List<Usuario> GetUsuarios()
        {
            return this.context.Usuarios.ToList();
        }

        public int ObtenerSiguienteIdUsuario()
        {
            int max = 0;
            if (this.context.Usuarios.Count() == 0)
            {
                max = 1;
            }
            else
            {
                max = this.context.Usuarios.Max(z => z.IdUsuario);
                if (max < 10)
                {
                    max = 10;
                }
                else
                {
                    max += 1;
                }
            }
            return max;
        }

        public List<Juego> PaginarJuegos(int posicion)
        {
            return this.context.Juegos.OrderBy(z => z.Titulo).Skip(posicion * 30).Take(30).ToList();
        }

        public List<Juego> PaginarJuegos(int posicion, List<Juego> juegos)
        {
            return juegos.OrderBy(z => z.Titulo).ToList();
        }

        public int RegistrarUsuario(String email, byte[] password, String salt, String username)
        {
            Random r = new Random();
            int aleat = r.Next(999999999);
            Usuario u = new Usuario();
            u.IdUsuario = this.ObtenerSiguienteIdUsuario();
            u.Email = email;
            u.Salt = salt;
            u.Role = "Usuario";
            u.Foto = "https://storageamg.blob.core.windows.net/fotoperfiles/pordefecto.jpeg";
            u.Password = password;
            u.Username = username;
            u.FechaNacimiento = DateTime.Parse("1900-01-01");
            u.Activo = 0;
            u.ListaDeseados = "";
            u.Nombre = "user" + aleat;
            u.Apellidos = "apellidos" + aleat;
            this.context.Usuarios.Add(u);
            this.context.SaveChanges();
            return u.IdUsuario;
        }

        public String ComprobarDisponibilidad(string email, string username)
        {
            String mensaje = "";
            if (this.context.Usuarios.SingleOrDefault(z => z.Email == email) != null || email.Length == 0)
            {
                mensaje = "El email está vacío o ya existe una cuenta vinculada a ese email";
            }
            else if (this.context.Usuarios.SingleOrDefault(z => z.Username == username) != null || username.Length == 0)
            {
                mensaje = "El nombre de usuario está vacío o ya existe una cuenta con ese nombre de usuario";
            }
            return mensaje;
        }

        public List<Juego> BuscarJuegos(string busqueda)
        {
            var consulta = from datos in context.Juegos where datos.Titulo.Contains(busqueda) select datos;
            return consulta.ToList();
        }

        public List<String> GetIdJuegoLista(int id)
        {
            Usuario u = this.context.Usuarios.SingleOrDefault(z => z.IdUsuario == id);
            String listacompleta = u.ListaDeseados;
            return listacompleta.Split(',').ToList();
        }

        public void AnyadirJuegoListaDeseados(String idjuego, int iduser)
        {
            Usuario u = this.context.Usuarios.SingleOrDefault(z => z.IdUsuario == iduser);
            String listacompleta = u.ListaDeseados;
            if (listacompleta == "" || listacompleta == null)
            {
                listacompleta += idjuego;
            }
            else
            {
                listacompleta += "," + idjuego;
            }
            u.ListaDeseados = listacompleta;
            this.context.SaveChanges();
        }
        public void EliminarJuegoListaDeseados(String idjuego, int iduser)
        {
            Usuario u = this.context.Usuarios.SingleOrDefault(z => z.IdUsuario == iduser);
            String listacompleta = u.ListaDeseados;
            if (listacompleta == "" || listacompleta == null)
            {
                listacompleta = "";
            }
            else if (u.ListaDeseados.IndexOf(",") != -1)
            {
                if (u.ListaDeseados.Split(',').Last() == idjuego.ToString())
                {
                    listacompleta = listacompleta.Replace("," + idjuego, "");
                }
                else
                {
                    listacompleta = listacompleta.Replace(idjuego + ",", "");
                }
            }
            else
            {
                listacompleta = "";
            }
            u.ListaDeseados = listacompleta;
            this.context.SaveChanges();
        }

        public void DarBajaCuenta(int id)
        {
            Usuario u = this.BuscarUsuario(id);
            u.Activo = 0;
            this.context.SaveChanges();
        }

        public void CambiarPassword(Usuario user)
        {
            Usuario u = this.BuscarUsuario(user.IdUsuario);
            byte[] passnuevo = user.Password;
            u.Password = passnuevo;
            this.context.SaveChanges();
        }

        public void EliminarUsuario(int id)
        {
            Usuario u = this.BuscarUsuario(id);
            this.context.Usuarios.Remove(u);
            this.context.SaveChanges();
        }
    }
}
