export const environment = {
  production: false,
  baseUrl: 'http://localhost:5067/api',

  UrlConstants: {
    Drug: {
      GetAllDrugs: 'Drug/get-all-drugs',
    },

    login: {
      login: 'login',
    },

    PurchaseOrder: {
      CreatePurchaseOrder: 'PurchaseOrder/create-purchase-order',
      FetchPurchaseOrders: 'PurchaseOrder/get-purchase-orders',
      FetchPurchaseOrderById: 'PurchaseOrder/get-purchase-order-by-id',
      FetchPurchaseOrderByDealer: 'PurchaseOrder/fetch-purchase-order-by-dealer',
      GetDraftPurchaseOrders: 'PurchaseOrder/get-draft-purchase-orders',
      ProcessPurchaseOrder: 'PurchaseOrder/process-purchase-order',
      ValidateProductAvailability: 'PurchaseOrder/validate-product-availability',
      SubmitPurchaseOrder: 'PurchaseOrder/submit-purchase-order',
      SavePurchaseOrderDraft: 'PurchaseOrder/save-purchase-order-draft',
      DeletePurchaseOrderDraft: 'PurchaseOrder/delete-purchase-order-draft',
    },
    Warehouse: {
      CreateWarehouse: 'Warehouse/create-warehouse',
      GetAllWarehouses: 'Warehouse/get-all-warehouses',
      GetWarehouseById: 'Warehouse/get-warehouse-by-id',
      GetWarehouseByDistributor: 'Warehouse/get-warehouse-by-distributor',
      UpdateWarehouse: 'Warehouse/update-warehouse',
      DeleteWarehouse: 'Warehouse/delete-warehouse',
    },

    User: {
      UpdateUserProfileAsync: 'user/updateuser',
      ChangePasswordAsync: 'user/changepassword',
      GetAllUsersByDealerIdAsync: 'user/get-all-users-by-dealer',
      InsertUserAsync: 'user/insert-user',
      CreateUserAsync: 'user/create-user',
      updateUserAsync: 'user/updateuser',
      deactivateUserAsync: 'user/deactivateuser',
    },

    Role: {
      GetAllRolesAsync: 'role/get-all-roles',
    },

    Dealer: {
      updateDealerAsync: 'dealer/updatedealer',
    },
  },
};
