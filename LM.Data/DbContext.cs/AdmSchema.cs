// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	public static partial class AdmSchema
	{
		public partial class DataContext
		{
			private readonly IDataContext _dataContext;

			public ITable<User>       Users       => _dataContext.GetTable<User>();
			public ITable<RefreshKey> RefreshKeys => _dataContext.GetTable<RefreshKey>();
			public ITable<Claim>      Claims      => _dataContext.GetTable<Claim>();

			public DataContext(IDataContext dataContext)
			{
				_dataContext = dataContext;
			}
		}

		#region Table Extensions
		public static User? Find(this ITable<User> table, Guid id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<User?> FindAsync(this ITable<User> table, Guid id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Claim? Find(this ITable<Claim> table, Guid id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Claim?> FindAsync(this ITable<Claim> table, Guid id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion

		[Table("user", Schema = "adm")]
		public class User
		{
			[Column("mail"                               )] public string? Mail          { get; set; } // text
			[Column("password"                           )] public string? Password      { get; set; } // text
			[Column("id"            , IsPrimaryKey = true)] public Guid    Id            { get; set; } // uuid
			[Column("user_name"                          )] public string? UserName      { get; set; } // text
			[Column("role"                               )] public short?  Role          { get; set; } // smallint
			[Column("normalize_name"                     )] public string? NormalizeName { get; set; } // text

			#region Associations
			/// <summary>
			/// claims_user_null_fk backreference
			/// </summary>
			[Association(ThisKey = nameof(Id), OtherKey = nameof(Claim.FUserId))]
			public IEnumerable<Claim> Claimsnullfks { get; set; } = null!;
			#endregion
		}

		[Table("refresh_keys", Schema = "adm")]
		public class RefreshKey
		{
			[Column("key"      , CanBeNull = false)] public string    Key      { get; set; } = null!; // text
			[Column("f_user_id"                   )] public Guid?     FUserId  { get; set; } // uuid
			[Column("n_expires"                   )] public DateTime? NExpires { get; set; } // timestamp (6) without time zone
			[Column("id"                          )] public int?      Id       { get; set; } // integer
		}

		[Table("claims", Schema = "adm")]
		public class Claim
		{
			[Column("id"       , IsPrimaryKey = true)] public Guid    Id      { get; set; } // uuid
			[Column("f_user_id"                     )] public Guid?   FUserId { get; set; } // uuid
			[Column("c_key"                         )] public string? CKey    { get; set; } // text
			[Column("c_value"                       )] public string? CValue  { get; set; } // text

			#region Associations
			/// <summary>
			/// claims_user_null_fk
			/// </summary>
			[Association(ThisKey = nameof(FUserId), OtherKey = nameof(User.Id))]
			public User? FUser { get; set; }
			#endregion
		}
	}
}
