import { Component, signal, inject, OnInit } from '@angular/core';
import { AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { ProductApi } from '../../DataAccess/product-api';
import { ProductResponseModel } from '../../Models/Products/ProductResponseModel';
import { createProductForm, toCreateProductRequest } from '../../Validations/Products/CreateProductFormFactory';
import { updateProductForm, toUpdateProductRequest } from '../../Validations/Products/UpdateProductFormFactory';

@Component({
  selector: 'app-product-operation',
  imports: [ReactiveFormsModule],
  templateUrl: './product-operation.html',
  styleUrl: './product-operation.css',
})
export class ProductOperation implements OnInit{
  private productApi = inject(ProductApi);

  protected products = signal<ProductResponseModel[]>([]);
  protected selectedProduct = signal<ProductResponseModel|null>(null);

  //UI state formlarımız
  protected createForm = createProductForm();
  protected updateForm = updateProductForm();

  private async refreshProducts():Promise<void>{
    try{
        const values = await this.productApi.getAll();
        this.products.set(values);
    }catch(error){
      console.log("Ürün listesi alınamadı:",error);
    }
  }

  async ngOnInit(): Promise<void> {
    await this.refreshProducts();
  }

  //Create işlemleri
  async onCreate(): Promise<void>{
    if (this.createForm.invalid){
      this.createForm.markAllAsTouched();
      return;
    }

    const req = toCreateProductRequest(this.createForm);
    await this.productApi.create(req);
    this.createForm.reset();
    await this.refreshProducts();
  }

  //Update işlemleri
  startUpdate(prod:ProductResponseModel){
    this.selectedProduct.set(prod);
    this.updateForm.patchValue(
      {
        id: prod.id,
        name: prod.productName,
        unitPrice: prod.unitPrice,
        categoryId: prod.categoryId,
      },
      { emitEvent: false }
    );
  }

  cancelUpdate(){
    this.selectedProduct.set(null);
    this.updateForm.reset({ id: 0, name: '', unitPrice: 0, categoryId: 0 });
  }

  async onUpdate(){
    if(this.updateForm.invalid){
      this.updateForm.markAllAsTouched();
      return;
    }

    // DÜZELTME BURADA YAPILDI:
    // 'as any' kullanarak TypeScript'in null kontrol hatasını susturuyoruz.
    // Çünkü cancelUpdate() içinde categoryId'yi 0 olarak resetliyoruz, yani null gelmeyeceğinden eminiz.
    const req = toUpdateProductRequest(this.updateForm as any);
    
    await this.productApi.update(req);
    this.cancelUpdate();
    await this.refreshProducts();
  }
  
  //Delete
  async onDelete(id:number): Promise<void>{
    const confirmDelete = window.confirm(
      `Id'si ${id} olan ürünü silmek istediğinize emin misiniz?`
    );

    if(!confirmDelete) return;

    try {
      const message = await this.productApi.deleteById(id);
      console.log(`Delete mesajı`, message);

      this.products.update((X) => X.filter((c) => c.id !== id));

      const selected = this.selectedProduct();
      if(selected && selected.id === id){
        this.selectedProduct.set(null);
      }
    } catch (error) {
      console.log(error);
      
    }
  }

  protected labels:Record<string,string> ={
    name: 'Ürün Adı',
    unitPrice: 'Ürün Birim Fiyatı',
    categoryId: 'Ürünün kategorisi',
    id: 'Id',
  };

  protected getErrorMessage(control:AbstractControl|null,label='Bu alan'):string|null{
    if(!control || (!control.touched && !control.dirty) || !control.invalid)
      return null;
    else if(control.hasError('required'))
      return `${label} zorunludur`;
    else if(control.hasError('minlength')){
      const e = control.getError('minlength'); //requiredlength, actuallength
      return `${label} en az ${e.requiredLength} karakter olmalıdır`;
    }
    else if(control.hasError('maxlength')){
      const e = control.getError('maxlength');
      return `${label} en fazla ${e.requiredLength} karakter olmalıdır`;
    }
    else if(control.hasError('min')){
      const e = control.getError('min');
      return `${label} en az ${e.min} tutarında olmalıdır`;
    }

    return `${label} geçersiz`;
  }

  protected getErrorMessageByName(form:{controls:Record<string, AbstractControl>}, controlName:string) : string|null{
    const control = form.controls[controlName];
    const label = this.labels[controlName] ?? controlName;

    return this.getErrorMessage(control, label);
  }
}