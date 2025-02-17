const productApiUrl = 'http://35.225.226.50';
const userApiUrl: string = 'http://35.223.163.117';
const searchApiUrl: string = 'http://34.59.239.110';

export const environment = {
  production: false,
  categoryproductApiUrl: `${productApiUrl}/api/category`, // GKE API
  productproductApiUrl: `${productApiUrl}/api/product`,
  homeproductApiUrl: `${productApiUrl}/api/home`,
  productItemproductApiUrl: `${productApiUrl}/api/product/item`,
  reviewUrl: `${productApiUrl}/api/product/review`,

  searchApiUrl: `${searchApiUrl}/api/search`,

  userApiUrl: `${userApiUrl}/api/user`,
  authUserApiUrl: `${userApiUrl}/api/user`,
};
