using NuGetCrackChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrackChecker.Repositories
{
    public interface IRepositoryProyecto
    {
        List<Juego> GetJuegos();
        Juego BuscarJuegoSlug(String slug);
        Juego BuscarJuegoId(String id);
        List<Juego> BuscarJuegos(String busqueda);
        List<Usuario> GetUsuarios();
        List<Usuario> BuscarUsuariosPorRole(String role);
        Usuario BuscarUsuario(int id);
        Usuario BuscarUsuario(String email, String password);
        String ComprobarDisponibilidad(String email, String username);
        int RegistrarUsuario(String email, byte[] password, String salt, String username);
        int ObtenerSiguienteIdUsuario();
        void EditarUsuario(int id, String email, String role, String foto, String nombre, String apellidos, DateTime fechaNac, int tlf, String username);
        List<Juego> PaginarJuegos(int posicion);
        List<Juego> PaginarJuegos(int posicion, List<Juego> juegos);
        void ActivarCuenta(int id);
        void EditarUsuario(int id, String role, int activo);
        void EditarJuego(String id, DateTime fechaCrack, String fechaLanz);
        List<String> GetIdJuegoLista(int id);
        void AnyadirJuegoListaDeseados(String idjuego, int iduser);
        void EliminarJuegoListaDeseados(String idjuego, int iduser);
        void DarBajaCuenta(int id);
        void CambiarPassword(Usuario u);
        void EliminarUsuario(int id);
    }
}
