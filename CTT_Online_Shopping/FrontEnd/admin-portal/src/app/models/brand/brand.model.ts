export interface BrandBase {
  name: string;
}
export interface BrandCommand extends BrandBase {}
export interface BrandView extends BrandBase {
  id: string;
}
