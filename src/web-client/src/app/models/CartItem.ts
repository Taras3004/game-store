export interface CartItem {
    Name: string;
    ShortDescription: string;
    Size: string;
    PriceUsd: number;
    DiscountedPrice?: number;
    Quantity: number;
}
