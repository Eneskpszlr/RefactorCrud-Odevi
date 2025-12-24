import { FormControl, FormGroup, Validators } from "@angular/forms";
import { UpdateCategoryRequestModel } from "../../Models/Categories/UpdateCategoryRequestModel";
import { baseCategoryForm } from "./BaseCategoryFormFactory";

export type UpdateCategoryForm = FormGroup<{
    id:FormControl<number>;
    name:FormControl<string>;
    description:FormControl<string>;
}>;

export function updateCategoryForm(){
    const base = baseCategoryForm();

    //#region
    /* Create'te dikkat ederseniz maxLength yok... Update'te validation logic değişebilir... Aynı fonksiyonel yapısına ek validator
    eklemek isteyebiliriz

    Bu inheritance değil

    */

    base.name.addValidators([Validators.maxLength(50)]);

    base.name.updateValueAndValidity({emitEvent:false});
    /* 
        updateValueAndValidity({emitEvent:false}) anlamı :
        Validator set'i artık değişti Angular'a kendini tekrar bir validate et  
        emitEvent:false => valueChanges tekrar tetiklenmesin diye...

        Aksi halde Form açıldığı zaman UI'da gereksiz validation event'leri olur
    */

        return new FormGroup({
            id:new FormControl(0, {
                nonNullable:true,
                validators: [Validators.required, Validators.min(1)],
            }),
            ...base,
        });
}

export function toUpdateCategoryRequest(form: UpdateCategoryForm): UpdateCategoryRequestModel{
    return {
        id: form.controls.id.value,
        categoryName: form.controls.name.value,
        description: form.controls.description.value,
    };
}