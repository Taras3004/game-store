import { Component, inject } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";

@Component({
    templateUrl: "support-request-form.component.html",
    imports: [ReactiveFormsModule],
    selector: "support-request-form",
    standalone: true
})
export class SupportRequestFormComponent{
    private fb: FormBuilder = inject(FormBuilder);

    form: FormGroup = this.fb.group({
        name: ['', Validators.required],
        email: ['', [Validators.required]],
        message: ['', Validators.required]
    });

    error: string | null = null;

    onSubmit(): void{
        this.form.markAllAsTouched();

        if(this.form.invalid){
            return;
        }

        this.error = "no error!";
        console.log("successfully");
    }
}