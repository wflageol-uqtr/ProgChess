import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import AuthLayout from "../../components/AuthLayout";
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
import { call } from "../../actions/auth";

const formSchema = z.object({
  email: z.string().email({ message: "Le courriel est invalide" }),
  password: z.string().min(2, { message: "Mot de passe invalide" }),
});

function AdminLogin() {
  const [error, setError] = useState("");
  const [isPending, startTransition] = useTransition();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    // TODO: Securite ?
    startTransition(async () => {
      const result = await call(() =>
        fetch("http://localhost:5290/api/auth/login", {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(values),
        })
      );

      form.reset();

      if (!result.success) {
        setError(result.error);
      } else {
        console.log("Login !");
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
                <FormItem>
                  <FormLabel>Mot de passe</FormLabel>
                  <FormControl>
                    <Input placeholder="*********" type="password" {...field} />
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

export default AdminLogin;
