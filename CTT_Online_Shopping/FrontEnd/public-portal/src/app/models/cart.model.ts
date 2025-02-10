export interface CartItem {
  variantType: string;
  sizeId?: string;
  sizeLabel: string;
  colorId?: string;
  imgUrl: string;
  discountedPrice: number;
  price: number;
  brand: string;
  name: string;
}
