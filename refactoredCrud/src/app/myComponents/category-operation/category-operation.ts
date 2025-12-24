import { Component, signal, inject, OnInit } from '@angular/core';
import { AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { CategoryApi } from '../../DataAccess/category-api';
import { CategoryResponseModel } from '../../Models/Categories/CategoryResponseModel';
import { createCategoryForm, toCreateCategoryRequest } from '../../Validations/Categories/CreateCategoryFormFactory';
import { updateCategoryForm, toUpdateCategoryRequest } from '../../Validations/Categories/UpdateCategoryFormFactory';

//Burada dikkat direkt http değil bir data-access service inject edeceğiz.

@Component({
  selector: 'app-category-operation',
  imports: [ReactiveFormsModule],
  templateUrl: './category-operation.html',
  styleUrl: './category-operation.css',
})
export class CategoryOperation implements OnInit{
  private categoryApi = inject(CategoryApi);

  protected categories = signal<CategoryResponseModel[]>([]);
  protected selectedCategory = signal<CategoryResponseModel|null>(null);

  //UI state formlarımız
  protected createForm = createCategoryForm();
  protected updateForm = updateCategoryForm();

  private async refreshCategories():Promise<void>{
    try{
        const values = await this.categoryApi.getAll();
        this.categories.set(values);
    }catch(error){
      console.log("Kategori listesi alınamadı:",error);
    }
  }

  async ngOnInit(): Promise<void> {
    await this.refreshCategories();
  }

  //Create işlemleri
  async onCreate(): Promise<void>{
    if (this.createForm.invalid){
      this.createForm.markAllAsTouched();
      return;
    }

    const req = toCreateCategoryRequest(this.createForm);
    await this.categoryApi.create(req);
    this.createForm.reset();
    await this.refreshCategories();
  }
}
