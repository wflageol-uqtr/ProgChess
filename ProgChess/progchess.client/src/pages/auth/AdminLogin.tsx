import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import Layout from "../../components/layout/AuthLayout";
import AuthCard from "../../components/card/AuthCard";
import { useForm } from "react-hook-form";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import Flash from "../../components/flash/Flash";
import { useState, useTransition } from "react";
import { Link, useNavigate } from "react-router";
import axios from "axios";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import { useAuth } from "../../providers/AuthProvider";
import { handleApiError } from "../../utils/apiErrorHandler";
import { apiUrl } from "../../utils/api";

const formSchema = z.object({
  email: z.string().email({ message: "Le courriel est invalide" }),
  password: z
    .string()
    .min(6, { message: "Mot de passe doit être 6 caractères" }),
});

function AdminLogin() {
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const [isPending, startTransition] = useTransition();
  const navigate = useNavigate();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });
  const { setToken } = useAuth();

  async function onSubmit(values: z.infer<typeof formSchema>) {
    startTransition(async () => {
      try {
        const response = await axios.post(`${apiUrl}/api/auth/login`, values);
        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        localStorage.setItem("user", response.data.userId);
        setToken(response.data.accessToken);
        navigate("/admin/exercise");
      } catch (error) {
        handleApiError(error, setError);
        form.reset();
      }
    });
  }

  return (
    <Layout>
      <AuthCard>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <Flash type={"error"} message={error} />
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Courriel</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="joedoe@email.com"
                      className="text-white"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className="text-red-600" />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem className="mb-0">
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
            <div className="flex justify-start">
              <Link
                to="/admin/forgot-password"
                className="text-sm text-white underline hover:text-green-300 transition-colors duration-200 mt-2"
              >
                Mot de passe oublié ?
              </Link>
            </div>

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
    </Layout>
  );
}

export default AdminLogin;
