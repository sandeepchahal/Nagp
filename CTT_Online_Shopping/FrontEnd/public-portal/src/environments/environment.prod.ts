export const environment = {
  production: true,
  categoryApiUrl:
    'http://productapi-admin-service.default.svc.cluster.local/api/category', // GKE API
  authApiUrl:
    'http://userapi-public-service.default.svc.cluster.local/api/user',
  productApiUrl:
    'http://productapi-public-service.default.svc.cluster.local/api/product',
  productItemApiUrl:
    'http://productapi-public-service.default.svc.cluster.local/api/product/item',
  searchApiUrl:
    'http://searchapi-public-service.default.svc.cluster.local/api/search',
  userApiUrl: 'http://userapi-public-service.default.svc.cluster.local/user',
};
