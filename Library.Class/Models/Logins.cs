﻿using Library.Class.Models;
using Library.Class.Resources;
using Library.Class.Utils;
using prmToolkit.NotificationPattern;
using prmToolkit.NotificationPattern.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Library.Class.Enum.EnumStatusLogin;

namespace Library.Class.Models
{
    [Table("TBL_LOGINS")]
    public class Logins : Notifiable
    {
        [Key]
        public int CodigoLogin { get; private set; }

        public string Login { get; private set; }

        public string Senha { get; private set; }

        public string SenhaAntiga { get; private set; }

        public StatusLogin Status { get; private set; }
        
        public ICollection<Usuarios> Usuario { get; private set; }

        public int? CodigoPerfil { get; private set; }
        [ForeignKey("CodigoPerfil")]
        public virtual Perfis Perfil { get; private set; }
        
        public Logins(string login,string senha, string senhaantiga)
        {
            this.Login = login;
            this.Senha = senha;
            this.SenhaAntiga = senhaantiga;
            this.Status = StatusLogin.Ativo;

            Usuario = new HashSet<Usuarios>();

            Validar();            
        }

        public Logins AlterarLogin(string login, string senha, int codigoperfil)
        {
            this.Login = login;
            this.Senha = senha;
            //this.SenhaAntiga = senhaantiga;
            this.CodigoPerfil = codigoperfil;
            this.Status = StatusLogin.Ativo;

            Usuario = new HashSet<Usuarios>();

            Validar();

            return this;
        }


        public void Validar()
        {
            new AddNotifications<Logins>(this)
             .IfNotAreEquals(Senha, SenhaAntiga, Message.A_X0_DEVE_SER_IGUAL_A_X11.ToFormat("Senha", "Senha Antiga"))
             .IfNullOrInvalidLength(x => x.Senha, 6, 32, "A senha deve ter entre 6 a 32 caracteres");

            if (IsValid())
            {
                this.Senha = this.Senha.ConvertToMD5();
                this.SenhaAntiga = this.SenhaAntiga.ConvertToMD5();
            }

        }

        public void DesativarLogin(int codigologin, string senha)
        {
            Usuario = new HashSet<Usuarios>();

            this.CodigoLogin = codigologin;
            this.Senha = senha;
            this.Status = StatusLogin.Inativo;

            Validar();
        }

        public Logins()
        {
            Usuario = new HashSet<Usuarios>();
        }

    }
}


