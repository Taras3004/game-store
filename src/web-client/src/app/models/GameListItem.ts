export interface GameItem {
    id: string;
    title: string;
    platform: string;
    price: number;
    discountedPrice?: number;
    imageUrl?: string;
}
