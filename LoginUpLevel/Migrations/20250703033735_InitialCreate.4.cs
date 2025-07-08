using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginUpLevel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_orderId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Products_productId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_customerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_statusId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "statusName",
                table: "Statuses",
                newName: "StatusName");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Statuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "Products",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "totalPrice",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "statusId",
                table: "Orders",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Orders",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Orders",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Orders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Orders",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Orders",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "StatusName");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_statusId",
                table: "Orders",
                newName: "IX_Orders_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_customerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "totalPrice",
                table: "OrderDetail",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "aMount",
                table: "OrderDetail",
                newName: "AMount");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "OrderDetail",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "OrderDetail",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_productId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Products_ProductId",
                table: "OrderDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_OrderId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Products_ProductId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "Statuses",
                newName: "statusName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Statuses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Products",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Products",
                newName: "color");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "totalPrice");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Orders",
                newName: "statusId");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Orders",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Orders",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Orders",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Orders",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "customerId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Orders",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "Orders",
                newName: "status");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                newName: "IX_Orders_statusId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                newName: "IX_Orders_customerId");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDetail",
                newName: "totalPrice");

            migrationBuilder.RenameColumn(
                name: "AMount",
                table: "OrderDetail",
                newName: "aMount");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetail",
                newName: "productId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderDetail",
                newName: "orderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_productId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_orderId",
                table: "OrderDetail",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Products_productId",
                table: "OrderDetail",
                column: "productId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_customerId",
                table: "Orders",
                column: "customerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_statusId",
                table: "Orders",
                column: "statusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
