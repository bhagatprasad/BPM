export const environment = {
  production: false,
  baseUrl: 'http://localhost:5067/api',

  UrlConstants: {
    Drug: {
      GetAllDrugs: 'drug',
    },
    login: {
      login: 'login',
    },
    PurchaseOrder: {
      CreatePurchaseOrder: 'PurchaseOrder/CreatePurchaseOrder',
      FetchPurchaseOrderByDealer: 'PurchaseOrder/FetchPurchaseOrderByDealer',
      FetchPurchaseOrderById: 'PurchaseOrder/FetchPurchaseOrderById',
    },
    User: {
      UpdateUserProfileAsync: 'user/updateuser',
      ChangePasswordAsync:'user/changepassword',
      GetAllUsersByDealerIdAsync:'user/get-all-users-by-dealer',
      InsertUserAsync:'user/insert-user',
      CreateUserAsync:'user/create-user'
    },
    Dealer:{
      updateDealerAsync:'dealer/updatedealer'
    }
  },
};
