﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SuperBob.Model
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class SuperBobEntities : DbContext
{
    public SuperBobEntities()
        : base("name=SuperBobEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<LoginClaim> LoginClaims { get; set; }

    public virtual DbSet<LoginSocialMedia> LoginSocialMedias { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PlayList> PlayLists { get; set; }

    public virtual DbSet<PlayListVideo> PlayListVideos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<VideoLibrary> VideoLibraries { get; set; }

}

}

