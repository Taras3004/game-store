import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CartItemComponent } from "../cart-item/cart-item.component";
import { CartItem } from "../../../models/CartItem";

@Component({
    selector: "cart-item-list",
    standalone: true,
    imports: [CartItemComponent],
    templateUrl: "./cart-item-list.component.html",
})
export class CartItemListComponent {
    @Input({ required: true }) items: CartItem[] = [];

    // Ці події слухатиме розумний компонент CartPage
    @Output() itemRemoved = new EventEmitter<CartItem>();
    @Output() quantityIncreased = new EventEmitter<CartItem>();
    @Output() quantityDecreased = new EventEmitter<CartItem>();

    onRemove(item: CartItem): void {
        this.itemRemoved.emit(item);
    }

    onIncrease(item: CartItem): void {
        this.quantityIncreased.emit(item);
    }

    onDecrease(item: CartItem): void {
        this.quantityDecreased.emit(item);
    }
}
