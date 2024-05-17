using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Shared.Constants
{
    public class SQLConstant
    {
        #region Product
        public const string Get_All_Products = @"SELECT *
                                                FROM Product
                                                ORDER BY RowId
                                                OFFSET @Skip ROWS FETCH NEXT 8 ROWS ONLY;";

        public const string Update_Product_By_Id = @"UPDATE Product
                                                    Set ProductName = @ProductName,
	                                                    ProductDescription = @ProductDescription, 
	                                                    ProductPrice = @Price
                                                    WHERE Product.RowId = @RowId";
        public const string DELETE_PRODUCT_BYID = @"DELETE FROM Product
                                                    WHERE RowId = @RowId";
        public const string Add_PRODUCT = @"INSERT INTO Product (ProductName, ProductDescription, ProductPrice,QuantityInStock,CreatedDate,ModifiedDate)
                                            VALUES (@ProductName, @ProductDescription, @Price, @QuantityInStock,@CreateDate,@ModifyDate)";
        public const string SEARCH_PRODUCT = @"
                                                SELECT *
                                                FROM Product
                                                WHERE [ProductName] COLLATE SQL_Latin1_General_CP1_CI_AI LIKE '%'+@searchValue+'%' COLLATE SQL_Latin1_General_CP1_CI_AI
                                                ORDER BY RowId ASC
                                                OFFSET @Skip ROWS
                                                FETCH NEXT 8 ROWS ONLY";

        #endregion

        #region OrderDetail
        public const string GET_ORDERDETAIL_BY_ID = @"SELECT 
                                                            OD.RowId,
                                                            P.ProductName, 
                                                            C.UserName, 
                                                            O.OrderDate, 
                                                            OD.Quantity, 
                                                            OD.UnitPrice
                                                        FROM 
                                                            [DTPaint].[dbo].[Order] AS O
                                                            INNER JOIN [DTPaint].[dbo].[User] AS C ON O.UserId = C.RowId
                                                            INNER JOIN [DTPaint].[dbo].[OrderDetail] AS OD ON O.RowId = OD.OrderId
                                                            INNER JOIN [DTPaint].[dbo].[Product] AS P ON OD.ProductId = P.RowId
                                                        WHERE
                                                            O.UserId = @RowId";
        public const string Update_ORDERDETAIL = @" UPDATE OrderDetail
                                                    SET Quantity = @Quantity,
	                                                    UnitPrice = @UnitPrice,
	                                                    Subtotal = @Subtotal,
	                                                    ModifiedDate = @ModifiedDate,
	                                                    ModifiedBy = @ModifiedBy
                                                    WHERE 
	                                                    RowId = @RowId";
        public const string ADD_ORDERDETAIL = @"INSERT INTO OrderDetail (Quantity,UnitPrice,Subtotal,ModifiedDate,ModifiedBy,CreatedDate,CreatedBy)
                                                VALUES (@Quantity,@UnitPrice,@Subtotal,@ModifiedDate,@ModifiedBy,@CreatedDate,@CreatedBy)";
        public const string DELETE_ORDERDETAIL_BY_ID = @"DELETE FROM OrderDetail
                                                         WHERE RowId = @RowId";
        #endregion

    }
}
