export interface CartItem {
  drugId: string;
  drugCode: string;
  drugName: string;
  genericName?: string;
  manufacturer?: string;
  brandName?: string;
  category?: string;
  packing?: string;
  strength?: string;
  quantity: number;
  imageUrl?: string;
}
