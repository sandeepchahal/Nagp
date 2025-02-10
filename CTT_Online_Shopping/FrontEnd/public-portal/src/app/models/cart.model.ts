export interface CartItem {
  productId: string;
  productItemId: string;
  variantType: string;
  sizeId?: string;
  sizeLabel: string;
  colorId?: string;
  imgUrl: string;
  discountedPrice: number;
  price: number;
  brand: string;
  name: string;
  orderCount: number;
  totalPrice: number;
}
