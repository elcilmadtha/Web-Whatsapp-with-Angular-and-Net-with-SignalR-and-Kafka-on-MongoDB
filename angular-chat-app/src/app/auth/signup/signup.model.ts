export interface UserSignUpRepoResponse {
  isSuccess: boolean;
  message: string;
  errorMessage: string[];
  validationMessage: string[];
}
