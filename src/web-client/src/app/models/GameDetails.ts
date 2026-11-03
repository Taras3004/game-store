export interface GameDetails {
    title: string;
    description?: string;
    price: number;
    discountedPrice?: number;
    rating: number;
    reviewsCount: number;
    platform: string;
    size: string;
    genres: string[];
}
