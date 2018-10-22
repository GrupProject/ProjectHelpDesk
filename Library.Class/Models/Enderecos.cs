﻿using prmToolkit.NotificationPattern;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using prmToolkit.NotificationPattern;
using prmToolkit.NotificationPattern.Extensions;

namespace Library.Class.Class
{
    [Table("TBL_ENDERECO_NOVO_LIGACAO")]
    public class Enderecos : Notifiable
    {
        [Key]
        public int CodigoEndereco { get; private set; }

        public string Rua { get; private set; }

        public virtual Usuarios Usuario { get; private set; }

        public Enderecos(int codigoendereco, string rua)
        {
            this.CodigoEndereco = codigoendereco;
            this.Rua = rua;

            new AddNotifications<Enderecos>(this)
        .IfNullOrInvalidLength(x => x.Rua, 5, 50, Resources.Message.X0_OBRIGATORIA_E_DEVE_CONTER_ENTRE_X1_E_X2_CARACTERES.ToFormat("Endereço", "5", "50"));

        }
    }
}
