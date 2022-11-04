import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export namespace CustomValidators {
  
  export function patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      return regex.test(control.value) ? null : error;
    };
  }
  
  export const containsNumber: ValidatorFn =
    patternValidator(/\d/, { containsNumber: true });

  export const containsSpecialCharacter: ValidatorFn =
    patternValidator(/[^a-zA-Z0-9]/, {
      containsSpecialCharacter: true,
    });

  export function containsUppercase(control: AbstractControl): ValidationErrors | null {
    if (!control.value) {
      return null;
    }
    return control.value.toLowerCase() !== control.value
      ? null
      : { containsUppercase: true };
  }

  export function containsLowercase(control: AbstractControl): ValidationErrors | null {
    if (!control.value) {
      return null;
    }
    return control.value.toUpperCase() !== control.value
      ? null
      : { containsLowercase: true };
  }

  export function containsNoWhitespace(control: AbstractControl): ValidationErrors | null {
    if (!control.value) {
      return null;
    }
    return new RegExp("\\s+").test(control.value)
      ? { containsNoWhitespace: true }
      : null;
  }

  export function passwordsMatch(group: FormGroup): ValidationErrors | null {
    const password = group.get('password')!.value;
    const confirmedPassword = group.get('confirmPassword')!.value;

    return password === confirmedPassword
      ? null
      : { passwordsMatch: true };
  }
}
