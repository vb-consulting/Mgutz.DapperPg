using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Mgutz.DapperPg.Models;
using Norm;
using System.Linq;

namespace Mgutz.DapperPg.Dal {

    public class ProductRepository : IProductRepository {

        private readonly DbConnection _dbConnection;

        public ProductRepository(DbConnection conn) {
            _dbConnection = conn;
        }

        public async Task<IEnumerable<Product>> GetAll() {
            var stmt = @"select * from products";

            return await _dbConnection.QueryAsync<Product>(stmt).ToListAsync();
        }

        public async ValueTask<Product> GetById(int id) {
            var stmt = @"select * from products where id = @id";

            return await _dbConnection.QueryAsync<Product>(stmt, id).FirstOrDefaultAsync();
        }

        public async ValueTask<Product> Add(Product entity) {
            var stmt = @"
                insert into products (name, cost) 
                values (@name, @cost) 
                returning id, created_at, updated_at
            ";

            return await _dbConnection.QueryAsync<Product>(stmt, entity.Name, entity.Cost).FirstOrDefaultAsync();
        }

        public async ValueTask<Product> Update(Product entity, int id) {
            var stmt = @"
                update products 
                set name = @name, cost = @cost, updated_at = now() 
                where id = @id 
                returning updated_at
            ";

            return await _dbConnection.QueryAsync<Product>(
                stmt,
                ("name", entity.Name), ("cost", entity.Cost), ("id", id)
            ).FirstOrDefaultAsync();
        }

        public async Task Remove(int id) {
            var stmt = @"delete from products where id = @id";

            await _dbConnection.ExecuteAsync(stmt, id);
        }

    }

}
