import { FormControl, FormGroupDirective, NgForm } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";

export class ParentErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.parent?.invalid && isSubmitted);
  }
}

export class DefaultErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    return !!(control && control.invalid && form && form.submitted);
  }
}
