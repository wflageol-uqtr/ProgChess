import { z } from "zod";
import AuthCard from "../../components/card/AuthCard";
import AuthLayout from "../../components/layout/AuthLayout";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Flash from "../../components/flash/Flash";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { useState, useTransition } from "react";
import { handleApiError } from "../../utils/apiErrorHandler";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import axios from "axios";
import { useNavigate, useSearchParams } from "react-router";
import { toast } from "sonner";
import { apiUrl } from "../../utils/api";

const formSchema = z
  .object({
    password: z
      .string()
      .min(8, {
        message: "Le mot de passe doit comporter au moins 8 caractères",
      })
      .refine(
        (value) => {
          return /[A-Z]/.test(value);
        },
        { message: "Le mot de passe doit contenir au moins une majuscule" }
      )
      .refine(
        (value) => /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(value),
        "Le mot de passe doit contenir au moins un caractère spécial"
      ),
    confirmation: z.string().min(8, {
      message:
        "La confirmation du mot de passe doit comporter au moins 8 caractères",
    }),
  })
  .refine((data) => data.password === data.confirmation, {
    message: "Les mots de passe ne correspondent pas",
    path: ["confirmation"],
  });

export default function ResetPassword() {
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmation, setShowConfirmation] = useState(false);
  const [error, setError] = useState("");
  const [isPending, startTransition] = useTransition();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      password: "",
      confirmation: "",
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    startTransition(async () => {
      try {
        await axios.put(`${apiUrl}/api/auth/reset-password`, {
          email: searchParams.get("email"),
          token: searchParams.get("activationToken"),
          password: values.password,
          confirmation: values.confirmation,
        });
        toast.success("Mot de passe modifié !");
        navigate("/admin/login");
      } catch (error) {
        handleApiError(error, setError);
        form.reset();
      }
    });
  }
  return (
    <AuthLayout>
      <AuthCard>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <Flash type={"error"} message={error} />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem className="my-4">
                  <FormLabel>Mot de passe</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Input
                        placeholder="*********"
                        type={showPassword ? "text" : "password"}
                        {...field}
                      />
                      <Button
                        className="absolute top-0 right-0 h-full px-3 py-2 hover:bg-transparent bg-transparent cursor-pointer"
                        type="button"
                        onClick={() => setShowPassword((prev) => !prev)}
                        disabled={
                          field.value === "" || field.value === undefined
                        }
                      >
                        {showPassword && field.value !== "" ? (
                          <EyeIcon className="w-4 h-4" aria-hidden="true" />
                        ) : (
                          <EyeOffIcon className="w-4 h-4" aria-hidden="true" />
                        )}
                      </Button>
                    </div>
                  </FormControl>
                  <FormMessage className="text-red-600" />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="confirmation"
              render={({ field }) => (
                <FormItem className="my-4">
                  <FormLabel>Confirmation</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Input
                        placeholder="*********"
                        type={showConfirmation ? "text" : "password"}
                        {...field}
                      />
                      <Button
                        className="absolute top-0 right-0 h-full px-3 py-2 hover:bg-transparent bg-transparent cursor-pointer"
                        type="button"
                        onClick={() => setShowConfirmation((prev) => !prev)}
                        disabled={
                          field.value === "" || field.value === undefined
                        }
                      >
                        {showConfirmation && field.value !== "" ? (
                          <EyeIcon className="w-4 h-4" aria-hidden="true" />
                        ) : (
                          <EyeOffIcon className="w-4 h-4" aria-hidden="true" />
                        )}
                      </Button>
                    </div>
                  </FormControl>
                  <FormMessage className="text-red-600" />
                </FormItem>
              )}
            />

            <Button
              type="submit"
              className="w-full bg-green-500 cursor-pointer hover:bg-green-600"
              disabled={isPending}
            >
              {isPending ? "Connexion..." : "Connexion"}
            </Button>
          </form>
        </Form>
      </AuthCard>
    </AuthLayout>
  );
}
