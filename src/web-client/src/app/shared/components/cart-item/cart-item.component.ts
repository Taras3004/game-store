import { CurrencyPipe } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CartItem } from "../../../models/CartItem";

@Component({
    templateUrl: "cart-item.component.html",
    selector: "cart-item",
    standalone: true,
    imports: [CurrencyPipe],
})
export class CartItemComponent {
    @Input({ required: true }) item!: CartItem;

    @Output() remove = new EventEmitter<CartItem>();
    @Output() increase = new EventEmitter<CartItem>();
    @Output() decrease = new EventEmitter<CartItem>();

    onRemove(): void {
        this.remove.emit(this.item);
    }

    onIncrease(): void {
        this.increase.emit(this.item);
    }

    onDecrease(): void {
        if (this.item.Quantity > 1) {
            this.decrease.emit(this.item);
        }
    }
}
