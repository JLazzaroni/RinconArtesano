﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace RinconArtesano.Models
{

    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    

    public partial class RinconArtesanoEntities : DbContext
    {
        public RinconArtesanoEntities()
            : base("name=RinconArtesanoEntities")
        {

        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<Files> Files { get; set; }

        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        public virtual DbSet<MessagesHijos> MessagesHijos { get; set; }

        public virtual DbSet<MessagesPadres> MessagesPadres { get; set; }

        public virtual DbSet<Denuncias> Denuncias { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Experiences> Experiences { get; set; }
    }

}

