using System;
using Microsoft.EntityFrameworkCore;

namespace AspTest1.Models
{
	public class MovieContext : DbContext
    {
		public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

		public DbSet<Movie> Movies { get; set; }
    } // end of class MovieContext
} // end of namespace
